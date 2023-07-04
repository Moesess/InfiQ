import uuid

from django.db import models


class ExamType(models.Model):
    """
        Type of Exam model, contains:
        uid - unique id,
        name - exam name e.g. EE.08, INF.02,
        text - description of an exam
    """

    e_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    e_name = models.CharField(max_length=20,)
    e_text = models.TextField()

    class Meta:
        ordering = ["uid"]
        get_latest_by = ["uid"]
        verbose_name = "ExamType"

    def __str__(self):
        return self.e_name
