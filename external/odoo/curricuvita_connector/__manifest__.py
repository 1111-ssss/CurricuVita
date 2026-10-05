{
    "name": "CurricuVita Connector",
    "version": "1.0.0",
    "summary": "Read-only viewer of CurricuVita position aggregates",
    "category": "Human Resources",
    "depends": ["base"],
    "data": [
        "security/ir.access.xml",
        "views/curricuvita_position_views.xml",
        "views/curricuvita_import_views.xml",
        "views/curricuvita_menus.xml",
    ],
    "installable": True,
    "application": True,
    "license": "LGPL-3",
}
