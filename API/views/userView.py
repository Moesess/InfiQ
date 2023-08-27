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
        top_results = TestResult.objects.values('tr_test__t_user').annotate(
            best_score=Max('tr_score')).order_by('-best_score')[:100]

        return Response(top_results)
