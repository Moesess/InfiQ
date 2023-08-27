import uuid

from django.db import models
from django.utils import timezone

from .test import Test


class TestResult(models.Model):
    """
    The TestResult model represents the result of a user's test.
    Each test result is associated with a specific user and test and records the start time, end time, and score.
    Fields:
    tr_uid - A unique identifier for the test result.
    tr_user - The user who completed the test.
    tr_test - The test that was completed.
    tr_date_start - The start time of the test.
    tr_date_end - The end time of the test.
    tr_score - The score the user achieved on the test. It should be calculated as correct answers * some time factor
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

    @property
    def calculate_score(self):
        score = self.tr_score * 1000 / self.duration.seconds
        return score

    def __str__(self):
        return f"{self.tr_date_start}: {self.tr_test.t_testType.tt_name} - {self.tr_score}"
