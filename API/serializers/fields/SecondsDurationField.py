from rest_framework import serializers
from datetime import timedelta


class SecondsDurationField(serializers.Field):
    """Niestandardowe pole serializatora, które przekształca wartość DurationField na sekundy."""

    def to_representation(self, value: timedelta) -> float:
        """Konwertuje DurationField na sekundy."""
        return round(value.total_seconds(), 2)

    def to_internal_value(self, data: int) -> timedelta:
        """Konwertuje sekundy na DurationField."""
        return timedelta(seconds=data)
