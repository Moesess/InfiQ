import uuid

from django.db import models

from .testType import TestType
from .question import Question


class Test(models.Model):
    """
        Test model, contains:
        uid - unique id,
        exam - type of exam that tests is made of,
        questions - m2m field that contains all the questions
    """

    t_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    t_testType = models.ForeignKey(TestType, on_delete=models.CASCADE)
    t_questions = models.ManyToManyField(Question)

    class Meta:
        ordering = ["t_uid"]
        get_latest_by = ["t_uid"]
        verbose_name = "Test"

    def __str__(self):
        return str(self.t_uid)