import uuid

from django.db import models
from django.utils import timezone

from .question import Question


class Answer(models.Model):
    """
    The Answer model represents a possible answer to a question.
    Each answer is associated with a specific question and can be marked as correct or not.
    Fields:
    a_uid - A unique identifier for the answer.
    a_text - The text of the answer itself.
    a_question - The question this answer is associated with.
    a_correct - A boolean indicating whether this answer is correct.
    a_created_at - The time the answer was created.
    """

    a_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    a_text = models.TextField(verbose_name="Text")
    a_question = models.ForeignKey(Question, related_name='answers', on_delete=models.CASCADE, verbose_name="Question")
    a_correct = models.BooleanField(verbose_name="Is correct?")
    a_created_at = models.DateTimeField(default=timezone.now)

    class Meta:
        ordering = ["a_uid"]
        get_latest_by = ["a_uid"]
        verbose_name = "Answer"

    def __str__(self):
        return self.a_text
