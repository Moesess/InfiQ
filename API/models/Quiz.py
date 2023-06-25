import uuid

from django.db import models

from API.models import Exam, Question


class Test(models.Model):
    """
        Quiz model, contains:
        uid - unique id,
        exam - type of exam that tests is made of,
        questions - m2m field that contains all the questions
    """

    t_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    t_exam = models.ForeignKey(Exam, on_delete=models.CASCADE)
    t_questions = models.ManyToManyField(Question)

    def __str__(self):
        return self.t_uid
