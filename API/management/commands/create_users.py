from django.contrib.auth.models import User
from django.core.management.base import BaseCommand


class Command(BaseCommand):
    help = 'Create Django users'

    def handle(self, *args, **options):
        try:
            # Create admin user
            User.objects.create_superuser('admin', 'admin@example.com', 'admin')

            # Create user1
            User.objects.create_user('user1', 'user1@example.com', 'user1')
            # Create user1
            User.objects.create_user('user2', 'user2@example.com', 'user2')

            self.stdout.write(self.style.SUCCESS('Django users created successfully.'))

        except Exception as inst:
            self.stdout.write(self.style.WARNING('ERROR Could not create users'))
