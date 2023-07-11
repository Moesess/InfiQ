import uuid

from django.db import models
from django.utils import timezone

from .testType import TestType


class Question(models.Model):
    """
        Question model, contains:
        uid - unique id,
        exam - type of exam e.g. EE.08, EE.09, foreign key from model Exam
        text - description of a question, text field
    """

    q_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    q_testType = models.ForeignKey(TestType, on_delete=models.CASCADE, verbose_name="Test type")
    q_text = models.TextField(verbose_name="Text")
    q_img = models.ImageField(blank=True, null=True)
    q_created_at = models.DateTimeField(default=timezone.now)

    class Meta:
        ordering = ["q_uid"]
        get_latest_by = ["q_uid"]
        verbose_name = "Question"

    def __str__(self):
        return self.q_text
