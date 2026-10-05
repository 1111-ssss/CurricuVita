#!/bin/sh
set -eu

DB_PORT="${DB_PORT:-5432}"

echo "Waiting for Postgres at $DB_HOST:$DB_PORT..."
until python3 -c "import socket; socket.create_connection(('$DB_HOST', $DB_PORT), timeout=3)" 2>/dev/null; do
  sleep 2
done

: "${ODOO_ADMIN_PASSWORD:?Set ODOO_ADMIN_PASSWORD env var}"
: "${DB_USER:?Set DB_USER env var}"
: "${DB_PASSWORD:?Set DB_PASSWORD env var}"

CONF=/var/lib/odoo/odoo.conf
cat > "$CONF" <<EOF
[options]
addons_path = /mnt/extra-addons
admin_passwd = $ODOO_ADMIN_PASSWORD
db_host = $DB_HOST
db_port = $DB_PORT
db_user = $DB_USER
db_password = $DB_PASSWORD
dbfilter = ^odoo$
http_port = ${PORT:-8069}
proxy_mode = True
workers = 0
EOF

export PGHOST="$DB_HOST" PGPORT="$DB_PORT" PGUSER="$DB_USER" PGPASSWORD="$DB_PASSWORD"

db_state() {
  python3 - <<'PYEOF'
import psycopg2

try:
    admin = psycopg2.connect(dbname="odoo", connect_timeout=5)
except psycopg2.OperationalError:
    print("missing")
else:
    cur = admin.cursor()
    cur.execute("SELECT 1 FROM pg_tables WHERE schemaname='public' AND tablename='ir_module_module'")
    if not cur.fetchone():
        print("empty")
    else:
        cur.execute("SELECT state FROM ir_module_module WHERE name='base'")
        row = cur.fetchone()
        if not row or row[0] != "installed":
            print("partial")
        else:
            cur.execute("SELECT state FROM ir_module_module WHERE name='curricuvita_connector'")
            row = cur.fetchone()
            print("ready" if row and row[0] == "installed" else "needs_connector")
    admin.close()
PYEOF
}

reset_db() {
  python3 -c "import psycopg2; c=psycopg2.connect(dbname='postgres'); c.autocommit=True; cur=c.cursor(); cur.execute('DROP DATABASE IF EXISTS odoo WITH (FORCE)'); cur.execute('CREATE DATABASE odoo'); c.close()"
}

STATE="$(db_state)"
echo "Database odoo state: $STATE"

if [ "$STATE" = "missing" ]; then
  echo "Creating database odoo..."
  python3 -c "import psycopg2; c=psycopg2.connect(dbname='postgres'); c.autocommit=True; c.cursor().execute('CREATE DATABASE odoo'); c.close()"
  STATE="empty"
fi

if [ "$STATE" = "partial" ]; then
  echo "Half-initialized database detected, dropping for a clean reinstall..."
  reset_db
  STATE="empty"
fi

if [ "$STATE" = "empty" ]; then
  echo "Initializing database odoo (base + curricuvita_connector, no demo)..."
  odoo -c "$CONF" -d odoo -i base,curricuvita_connector --stop-after-init --without-demo=all
  echo "Initialization done."
elif [ "$STATE" = "needs_connector" ]; then
  echo "Installing curricuvita_connector..."
  odoo -c "$CONF" -d odoo -i curricuvita_connector --stop-after-init --without-demo=all
  echo "Module installed."
fi

echo "Syncing admin password from env..."
( timeout 300 odoo -c "$CONF" -d odoo shell --no-http <<'PYEOF' || echo "WARN: admin password sync failed, continuing with stored password"
import os
admin = env['res.users'].search([('login', '=', 'admin')], limit=1)
if admin:
    admin.write({'password': os.environ['ODOO_ADMIN_PASSWORD']})
    env.cr.commit()
    print('ADMIN_PASSWORD_SYNCED')
else:
    print('ADMIN_USER_NOT_FOUND')
PYEOF
)

exec odoo -c "$CONF" "$@"
