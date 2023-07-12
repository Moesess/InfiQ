import uuid

from django.db import models
from django.utils import timezone

from .test import Test


class TestResult(models.Model):
    """
        Results model, contains:
        uid - unique id,
        test - test object had all the questions,
        date_start - starting datetime for test,
        date_end - ending datetime for test,
        score - final score that will be defined TODO
    """

    tr_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    tr_test = models.ForeignKey(Test, on_delete=models.CASCADE, verbose_name="Test")
    tr_date_start = models.DateTimeField(verbose_name="Start date", default=timezone.now)
    tr_date_end = models.DateTimeField(null=True, verbose_name="End date")
    tr_score = models.IntegerField(verbose_name="Score")
    tr_isDone = models.BooleanField(default=False)

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
        return f"{self.tr_date_start}: {self.tr_test.t_testType.tt_name} - {self.tr_score}"
