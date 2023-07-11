from rest_framework import serializers
from API.models import Test
from API.serializers.fields import CustomDateTimeField


class TestSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='t_uid', read_only=True)
    testType = serializers.CharField(source='t_testType.tt_uid', read_only=True)
    questions = serializers.StringRelatedField(source='t_questions', many=True)
    created_at = CustomDateTimeField(source='t_created_at', read_only=True)

    class Meta:
        model = Test
        fields = ['uid', 'testType', 'questions', 'created_at']
