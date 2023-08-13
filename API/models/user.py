from datetime import datetime

from django.contrib.auth.models import AbstractUser
from django.db import models


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

    In addition to the fields listed above, the user also has a username, email and password
    from the inherited AbstractUser class.
    """
    number_of_tests = models.PositiveIntegerField(default=0, verbose_name="Number of tests")
    best_score = models.IntegerField(default=0, verbose_name="Best score")
    best_time = models.DateTimeField(default=datetime.now, verbose_name="Best time")
    correct_answers = models.PositiveIntegerField(default=0, verbose_name="Correct answers")
    all_answers = models.PositiveIntegerField(default=0, verbose_name="All answers")

    @property
    def accuracy(self):
        if self.all_answers > 0:
            return self.correct_answers / self.all_answers

    class Meta:
        ordering = ["username"]
        get_latest_by = ["username"]
        verbose_name = "User"

    def __str__(self):
        return (f"Name: {self.username} Email: {self.email} Number of tests: {self.number_of_tests} "
                f"Best score: {self.best_score} Best time: {self.best_time} Correct answers: {self.correct_answers} "
                f"All answers: {self.all_answers} Accuracy: {self.accuracy}")
