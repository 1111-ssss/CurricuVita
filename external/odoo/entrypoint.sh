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
http_port = ${PORT:-8069}
proxy_mode = True
workers = 0
EOF

exec odoo -c "$CONF" "$@"
