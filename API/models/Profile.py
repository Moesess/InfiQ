import uuid

from django.db import models


class Profile(models.Model):
    """
        User profile model, contains:
        uid - unique id,
    """
    p_uid = models.UUIDField(default=uuid.uuid4, editable=False, unique=True, primary_key=True)

    class Meta:
        ordering = ["uid"]
        get_latest_by = ["uid"]
        verbose_name = "Profile"
