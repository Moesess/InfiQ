import uuid

from django.db import models

from .question import Question


class Answer(models.Model):
    """
        Answer model, contains:
        uid - unique id,
        text - description of an Answer,
        question - Foreign key on Question Model that answer is binded to,
        correct - Bool, if answer is correct
    """

    a_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True, verbose_name="UID")
    a_text = models.TextField(verbose_name="Text")
    a_question = models.ForeignKey(Question, related_name='answers', on_delete=models.CASCADE, verbose_name="Question")
    a_correct = models.BooleanField(verbose_name="Is correct?")

    class Meta:
        ordering = ["a_uid"]
        get_latest_by = ["a_uid"]
        verbose_name = "Answer"

    def __str__(self):
        return self.a_text
