import uuid

from django.db import models


class TestType(models.Model):
    """
        Type of Exam model, contains:
        uid - unique id,
        name - exam name e.g. EE.08, INF.02,
        text - description of an exam
    """

    tt_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    tt_name = models.CharField(max_length=20, verbose_name="Name")
    tt_text = models.TextField(verbose_name="Text")

    class Meta:
        ordering = ["tt_uid"]
        get_latest_by = ["tt_uid"]
        verbose_name = "ExamType"

    def __str__(self):
        return self.tt_name
