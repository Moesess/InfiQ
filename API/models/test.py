import uuid

from django.contrib.auth.models import User
from django.db import models
from django.utils import timezone

from .question import Question
from .testType import TestType


class Test(models.Model):
    """
    The Test model represents a single instance of a test.
    Each test is associated with a specific TestType and has a set of questions.
    Fields:
    t_uid - A unique identifier for the test.
    t_testType - The type of test this test instance is associated with.
    t_questions - The set of questions this test instance contains.
    t_created_at - The time the test instance was created.
    """

    t_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    t_testType = models.ForeignKey(TestType, on_delete=models.CASCADE, verbose_name="Test type")
    t_questions = models.ManyToManyField(Question, verbose_name="Questions")
    t_created_at = models.DateTimeField(default=timezone.now)
    t_user = models.ForeignKey(User, on_delete=models.CASCADE, verbose_name="User")

    class Meta:
        ordering = ["t_uid"]
        get_latest_by = ["t_uid"]
        verbose_name = "Test"

    def __str__(self):
        return str(self.t_uid)

