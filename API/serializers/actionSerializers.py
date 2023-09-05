from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import serializers

from API.models import User
from API.serializers.fields import SecondsDurationField


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class RandomQuestionAnswerSerializer(serializers.Serializer):
    test_uid = serializers.UUIDField()
    question_uid = serializers.UUIDField()
    answer_uid = serializers.UUIDField()


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class TestValidateSerializer(serializers.Serializer):
    test_uid = serializers.UUIDField()
    answers = serializers.DictField(child=serializers.UUIDField())


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class TopScoreUserSerializer(serializers.ModelSerializer):
    best_score = serializers.IntegerField()
    duration = SecondsDurationField()

    class Meta:
        model = User
        fields = ('u_uid', 'username', 'best_score', 'duration')
