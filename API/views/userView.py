from django.db.models import Max
from rest_framework import viewsets
from rest_framework.decorators import action
from rest_framework.response import Response

from ..models import User, TestResult
from ..serializers import UserSerializer


class UserView(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer

    @action(detail=False, methods=['GET'])
    def top_scores(self, request):
        # Pobierz 100 użytkowników o najwyższym wyniku
        top_users = User.objects.order_by('-u_best_score')[:100]
        serialized_users = UserSerializer(top_users, many=True)
        return Response(serialized_users.data)
