import uuid

from django.db import models
from django.utils import timezone

from .testType import TestType


class Question(models.Model):
    """
    The Question model represents a single question in a test.
    Each question is associated with a specific TestType, has a unique text, and can have an image associated with it.
    Fields:
    q_uid - A unique identifier for the question.
    q_testType - The type of test this question is associated with.
    q_text - The text of the question itself.
    q_img - An optional image associated with the question.
    q_created_at - The time the question was created.
    q_id - A unique ID for the question, automatically generated upon saving.
    """

    q_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    q_testType = models.ForeignKey(TestType, on_delete=models.CASCADE, verbose_name="Test type")
    q_text = models.TextField(verbose_name="Text")
    q_img = models.ImageField(blank=True, null=True)
    q_created_at = models.DateTimeField(default=timezone.now)
    q_id = models.CharField(max_length=255, editable=False, blank=True, unique=True)

    class Meta:
        ordering = ["q_uid"]
        get_latest_by = ["q_uid"]
        verbose_name = "Question"

    def __str__(self):
        return self.q_text

    def save(self, *args, **kwargs):
        if not self.q_id:
            count = Question.objects.filter(q_testType__tt_name=self.q_testType.tt_name).count()
            self.q_id = f"Q{count + 1}_{self.q_testType.tt_name}"
        super().save(*args, **kwargs)
