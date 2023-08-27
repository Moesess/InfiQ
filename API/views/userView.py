from rest_framework import viewsets

from ..models import User
from ..serializers import UserSerializer


class UserView(viewsets.ModelViewSet):
    queryset = User.objects.all().order_by('-best_score')
    serializer_class = UserSerializer
