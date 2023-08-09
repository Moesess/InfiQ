from rest_framework import serializers


class FinalScoreField(serializers.Field):
    def to_representation(self, value):
        return int(value)
