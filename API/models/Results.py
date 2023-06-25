import uuid

from django.db import models
from django.contrib.auth.models import User

from API.models import Quiz


class Result(models.Model):
    """
        Results model, contains:
        uid - unique id,
        user - user that did the test and got result,
        quiz - quiz object had all the questions,
        date_start - starting datetime for test,
        date_end - ending datetime for test,
        score - final score that will be defined TODO
    """

    r_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    r_user = models.ForeignKey(User, on_delete=models.CASCADE)
    r_quiz = models.ForeignKey(Quiz, on_delete=models.CASCADE)
    t_date_start = models.DateTimeField(auto_now_add=True)
    t_date_end = models.DateTimeField(null=True)
    t_score = models.IntegerField()

    def __str__(self):
        return f"{self.r_user.username}: {self.r_quiz.name} - {self.t_score}"
