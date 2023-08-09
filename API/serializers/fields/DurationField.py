from datetime import timedelta

from rest_framework import serializers


class DurationField(serializers.Field):
    def to_representation(self, value):
        return str(value)[:11]

    def to_internal_value(self, data):
        days, time = data.split(", ")
        hours, minutes, seconds = map(int, time.split(":"))
        return timedelta(days=int(days.split(" ")[0]), hours=hours, minutes=minutes, seconds=seconds)
