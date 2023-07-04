import uuid

from django.db import models
from django.contrib.auth.models import User

from .test import Test


class TestResult(models.Model):
    """
        Results model, contains:
        uid - unique id,
        user - user that did the test and got result,
        quiz - quiz object had all the questions,
        date_start - starting datetime for test,
        date_end - ending datetime for test,
        score - final score that will be defined TODO
    """

    tr_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)
    tr_user = models.ForeignKey(User, on_delete=models.CASCADE)
    tr_test = models.ForeignKey(Test, on_delete=models.CASCADE)
    tr_date_start = models.DateTimeField(auto_now_add=True)
    tr_date_end = models.DateTimeField(null=True)
    tr_score = models.IntegerField()

    class Meta:
        ordering = ["tr_uid"]
        get_latest_by = ["tr_uid"]
        verbose_name = "TestResult"

    @property
    def duration(self):
        if self.tr_date_end:
            return self.tr_date_end - self.tr_date_start
        return None

    def __str__(self):
        return f"{self.tr_user.username}: {self.tr_test.t_testType.tt_name} - {self.tr_score}"
