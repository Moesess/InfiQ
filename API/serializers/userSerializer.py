from rest_framework import serializers

from API.models import User
from API.serializers.fields import SecondsDurationField


class UserSerializer(serializers.ModelSerializer):

    uid = serializers.UUIDField(source='u_uid', read_only=True)
    name = serializers.CharField(source='username', read_only=True)
    number_of_tests = serializers.IntegerField(source='u_number_of_tests')
    best_score = serializers.IntegerField(source='u_best_score')
    best_time = SecondsDurationField(source='u_best_time')
    correct_answers = serializers.IntegerField(source='u_correct_answers')
    all_answers = serializers.IntegerField(source='u_all_answers')
    accuracy = serializers.FloatField(read_only=True)

    class Meta:
        model = User
        fields = ['uid', 'name', 'number_of_tests',
                  'best_score', 'best_time', 'correct_answers', 'all_answers', 'accuracy']
