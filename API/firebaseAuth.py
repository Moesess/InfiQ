from rest_framework.authentication import BaseAuthentication
from rest_framework.exceptions import AuthenticationFailed
from firebase_admin import auth

from API.models.user import User


class FirebaseAuthentication(BaseAuthentication):
    def authenticate(self, request):
        auth_header = request.headers.get('Authorization')
        # print(auth_header)
        if not auth_header:
            return None

        token = auth_header.split(' ')[1]
        try:
            decoded_token = auth.verify_id_token(token)
            user_name = decoded_token["name"]
            email = decoded_token["email"]
            # print(f"TOKEN: {decoded_token}")
            # print(f"IMIE: {user_name}")

            user, created = User.objects.get_or_create(username=user_name, email=email)
            if created:
                user.email = decoded_token.get('email', '')
                user.username = decoded_token.get('name', '')
                user.save()

            return user, user.u_uid
        except Exception as e:
            raise AuthenticationFailed(f'Invalid Firebase token {e}')
