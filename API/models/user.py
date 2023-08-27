from django.contrib.auth.models import AbstractUser
from django.db import models
from django.utils import timezone


class User(AbstractUser):
    """
    The User model represents the user.

    Fields:
    number_of_tests - The number of tests completed by user
    best_score - The best score earned by user in one of tests
    best_time - The best time earned by user in one of tests
    correct_answers - The sum of correct answers given by user in all of his tests
    all_answers - The sum of all answers given by user in all of his tests
    accuracy - the ratio of correct answers to all answers given by the user in all his tests
    """
    u_number_of_tests = models.PositiveIntegerField(default=0, verbose_name="Number of tests")
    u_best_score = models.IntegerField(default=0, verbose_name="Best score")
    u_best_time = models.DurationField(null=True, blank=True, verbose_name="Best time")
    u_correct_answers = models.PositiveIntegerField(default=0, verbose_name="Correct answers")
    u_all_answers = models.PositiveIntegerField(default=0, verbose_name="All answers")

    @property
    def accuracy(self) -> float:
        if self.u_all_answers > 0:
            return self.u_correct_answers / self.u_all_answers
        return 0

    class Meta:
        ordering = ["username"]
        get_latest_by = ["username"]
        verbose_name = "User"

    def __str__(self):
        return f"Name: {self.username}"
