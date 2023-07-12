from django.db.models.signals import post_save
from django.dispatch import receiver

from API.models import Test, TestResult


@receiver(post_save, sender=Test)
def create_test_result(sender, instance, created, **kwargs):
    if created:
        TestResult.objects.create(tr_test=instance, tr_score=0)
