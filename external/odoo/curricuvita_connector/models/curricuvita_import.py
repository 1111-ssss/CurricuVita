import requests

from odoo import fields, models
from odoo.exceptions import UserError


EXPORT_PATH = "/api/external/positions/"
REQUEST_TIMEOUT = 30


class CurricuvitaImport(models.TransientModel):
    _name = "curricuvita.import"
    _description = "Import position aggregates from CurricuVita"

    base_url = fields.Char(required=True)
    api_token = fields.Char(required=True)

    def action_import(self):
        self.ensure_one()
        try:
            response = requests.get(
                self.base_url.rstrip("/") + EXPORT_PATH + self.api_token.strip(),
                timeout=REQUEST_TIMEOUT,
            )
        except requests.RequestException as e:
            raise UserError(str(e))
        if response.status_code == 401:
            raise UserError("Invalid API token.")
        if not response.ok:
            raise UserError("Import failed: HTTP %s." % response.status_code)
        data = response.json()
        position = self.env["curricuvita.position"].search(
            [("external_id", "=", data["positionId"])], limit=1
        )
        values = {
            "name": data["title"],
            "external_id": data["positionId"],
            "company": data.get("company"),
            "level": data.get("level"),
            "published_cv_count": data.get("publishedCvCount", 0),
            "last_sync": fields.Datetime.now(),
        }
        if position:
            position.write(values)
            position.attribute_ids.unlink()
            position.numeric_ids.unlink()
            position.text_ids.unlink()
        else:
            position = self.env["curricuvita.position"].create(values)
        for attr in data.get("attributes", []):
            self.env["curricuvita.attribute"].create(
                {
                    "position_id": position.id,
                    "external_attr_id": attr["attributeDefinitionId"],
                    "name": attr["name"],
                    "category": attr.get("category"),
                    "data_type": attr.get("dataType"),
                    "required": attr.get("isRequired", False),
                }
            )
        for agg in data.get("numericAggregates", []):
            self.env["curricuvita.numeric"].create(
                {
                    "position_id": position.id,
                    "external_attr_id": agg["attributeDefinitionId"],
                    "name": agg["name"],
                    "count": agg.get("count", 0),
                    "average": agg.get("average", 0.0),
                    "min": agg.get("min", 0.0),
                    "max": agg.get("max", 0.0),
                }
            )
        for agg in data.get("textAggregates", []):
            text = self.env["curricuvita.text"].create(
                {
                    "position_id": position.id,
                    "external_attr_id": agg["attributeDefinitionId"],
                    "name": agg["name"],
                    "total": agg.get("totalCount", 0),
                }
            )
            for top in agg.get("topValues", []):
                self.env["curricuvita.text.value"].create(
                    {
                        "text_id": text.id,
                        "name": top["value"],
                        "count": top.get("count", 0),
                    }
                )
        return {
            "type": "ir.actions.act_window",
            "res_model": "curricuvita.position",
            "res_id": position.id,
            "view_mode": "form",
        }
