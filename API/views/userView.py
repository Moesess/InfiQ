from django.db.models import F, OuterRef, Subquery
from rest_framework import viewsets, status
from rest_framework.decorators import action
from rest_framework.response import Response

from ..models import User, TestResult
from ..serializers import UserSerializer, TopScoreUserSerializer


class UserView(viewsets.ModelViewSet):
    queryset = User.objects.all().order_by('-u_best_score')
    serializer_class = UserSerializer

    def list(self, request, *args, **kwargs):
        response = super().list(request, *args, **kwargs)
        data = {
            'auth': request.auth,
            'results': response.data
        }
        response.data = data
        return response

    @action(detail=False, methods=['GET'])
    def top_scores(self, request):
        test_type = request.query_params.get('test_type', None)

        if not test_type:
            return Response({"error": "Test type is required."}, status=status.HTTP_400_BAD_REQUEST)

        best_scores = TestResult.objects.filter(
            tr_test__t_testType__tt_name=test_type,
            tr_test__t_user=OuterRef('u_uid')
        ).order_by('-tr_final_score').annotate(
            duration=F('tr_date_end') - F('tr_date_start'))

        top_users = User.objects.annotate(
            best_score=Subquery(best_scores.values('tr_final_score')[:1]),
            duration=Subquery(best_scores.values('duration')[:1])
        ).order_by('-u_best_score')[:100]

        serialized_users = TopScoreUserSerializer(top_users, many=True)

        return Response(serialized_users.data)
