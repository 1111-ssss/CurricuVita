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
  python3 - "$@" <<'PYEOF'
import sys
import psycopg2

dbname = sys.argv[1]
try:
    conn = psycopg2.connect(dbname=dbname, connect_timeout=5)
except psycopg2.OperationalError:
    print("missing")
    return
cur = conn.cursor()
cur.execute("SELECT 1 FROM pg_tables WHERE schemaname='public' AND tablename='ir_module_module'")
print("ready" if cur.fetchone() else "empty")
conn.close()
PYEOF
}

STATE="$(db_state odoo)"
echo "Database odoo state: $STATE"

if [ "$STATE" = "missing" ]; then
  echo "Creating database odoo..."
  python3 -c "import psycopg2; c=psycopg2.connect(dbname='postgres'); c.autocommit=True; c.cursor().execute('CREATE DATABASE odoo')"
  STATE="empty"
fi

if [ "$STATE" = "empty" ]; then
  echo "Initializing database odoo (base + curricuvita_connector, no demo)..."
  odoo -c "$CONF" -d odoo -i base,curricuvita_connector --stop-after-init --without-demo=all
  echo "Initialization done."
fi

exec odoo -c "$CONF" "$@"
