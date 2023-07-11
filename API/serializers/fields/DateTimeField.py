from rest_framework import serializers


class CustomDateTimeField(serializers.DateTimeField):
    def to_representation(self, value):
        return value.strftime("%Y-%m-%d %H:%M:%S")
