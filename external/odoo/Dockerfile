FROM odoo:18

USER root
COPY curricuvita_connector /mnt/extra-addons/curricuvita_connector
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh \
 && chown -R odoo:odoo /mnt/extra-addons/curricuvita_connector

USER odoo
ENTRYPOINT ["/entrypoint.sh"]
