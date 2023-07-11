from rest_framework import serializers


class SubmitAnswerSerializer(serializers.Serializer):
    question_uid = serializers.UUIDField()
    answer_uid = serializers.UUIDField()
