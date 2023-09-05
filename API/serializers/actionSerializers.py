from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import serializers

from API.models import User
from API.serializers.fields import SecondsDurationField


class RandomQuestionAnswerSerializer(serializers.Serializer):
    test_uid = serializers.UUIDField()
    question_uid = serializers.UUIDField()
    answer_uid = serializers.UUIDField()


class TestValidateSerializer(serializers.Serializer):
    test_uid = serializers.UUIDField()
    answers = serializers.DictField(child=serializers.UUIDField())


class TopScoreUserSerializer(serializers.ModelSerializer):
    best_score = serializers.IntegerField()
    duration = SecondsDurationField()

    class Meta:
        model = User
        fields = ('u_uid', 'username', 'best_score', 'duration')
