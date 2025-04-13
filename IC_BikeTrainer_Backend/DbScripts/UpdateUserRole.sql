-- Update a user's role
UPDATE UsersTable
-- Role == 0 -> User, Role == 1 -> Admin
SET Role = 1
WHERE Username = 'alex';
