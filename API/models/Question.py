import uuid

from django.db import models
from API.models import Exam


class Question(models.Model):
    """
        Question model, contains:
        uid - unique id,
        exam - type of exam e.g. EE.08, EE.09, foreign key from model Exam
        text - description of a question, text field
    """

    q_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    q_exam = models.ForeignKey(Exam, on_delete=models.CASCADE)
    q_text = models.TextField()

    class Meta:
        ordering = ["uid"]
        get_latest_by = ["uid"]
        verbose_name = "Question"

    def __str__(self):
        return self
