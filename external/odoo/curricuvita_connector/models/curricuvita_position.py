from odoo import fields, models


class CurricuvitaPosition(models.Model):
    _name = "curricuvita.position"
    _description = "Imported CurricuVita position"
    _order = "name"

    name = fields.Char(required=True)
    external_id = fields.Integer(required=True, index=True)
    company = fields.Char()
    level = fields.Char()
    published_cv_count = fields.Integer()
    last_sync = fields.Datetime()
    attribute_ids = fields.One2many("curricuvita.attribute", "position_id")
    numeric_ids = fields.One2many("curricuvita.numeric", "position_id")
    text_ids = fields.One2many("curricuvita.text", "position_id")


class CurricuvitaAttribute(models.Model):
    _name = "curricuvita.attribute"
    _description = "Imported CurricuVita attribute"

    position_id = fields.Many2one("curricuvita.position", required=True, ondelete="cascade")
    external_attr_id = fields.Integer(required=True)
    name = fields.Char(required=True)
    category = fields.Char()
    data_type = fields.Char(required=True)
    required = fields.Boolean()


class CurricuvitaNumeric(models.Model):
    _name = "curricuvita.numeric"
    _description = "Imported numeric aggregate"

    position_id = fields.Many2one("curricuvita.position", required=True, ondelete="cascade")
    external_attr_id = fields.Integer(required=True)
    name = fields.Char(required=True)
    count = fields.Integer()
    average = fields.Float(digits=(16, 2))
    min = fields.Float(digits=(16, 2))
    max = fields.Float(digits=(16, 2))


class CurricuvitaText(models.Model):
    _name = "curricuvita.text"
    _description = "Imported text aggregate"

    position_id = fields.Many2one("curricuvita.position", required=True, ondelete="cascade")
    external_attr_id = fields.Integer(required=True)
    name = fields.Char(required=True)
    total = fields.Integer()
    value_ids = fields.One2many("curricuvita.text.value", "text_id")


class CurricuvitaTextValue(models.Model):
    _name = "curricuvita.text.value"
    _description = "Most popular text value"

    text_id = fields.Many2one("curricuvita.text", required=True, ondelete="cascade")
    name = fields.Char(required=True)
    count = fields.Integer()
