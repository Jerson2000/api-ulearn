
# ULearn - You need to learn; oy.

Aims to demonstrate the **Domain Driven Design Architecture (DDD)** and best practices.

### 📌 To Do
- [x] JWT
- [x] Role-Based Access Control (RBAC)
- [x] Caching (memory & redis)
- [x] Rate Limiting
- [x] Prevent Cross Site Request Forgery
- [x] Websocket
- [x] E-Mail Service
- [x] Storage (local & 3rd party)
- [x] API Documentation - Swagger

## API Endpoints

### Authentication

- **GET** `/api/auth/token` → Fetch CSRF token  
- **POST** `/api/auth/signup` → User registration  
- **POST** `/api/auth/login` → User login (returns accessToken)  
- **POST** `/api/auth/refresh` → Refresh access token  
- **GET** `/api/auth/me` → Get current authenticated user  

### Users

- **GET** `/api/users` → Get all users (admin only)  
- **POST** `/api/users` → Create a new user (admin only)  
- **GET** `/api/users/{userId}` → Get user by ID  
- **PUT** `/api/users/{userId}` → Update user by ID (admin only)  

### Categories

- **GET** `/api/categories` → List all categories  
- **POST** `/api/categories` → Create new category  

### Courses

- **GET** `/api/courses` → Get paged list of courses  
- **GET** `/api/courses/{courseId}` → Get course details  
- **POST** `/api/courses` → Create new course  
- **PUT** `/api/courses/{courseId}` → Update course  
- **POST** `/api/courses/enroll/{courseId}` → Enroll current user in course  

### Modules

- **GET** `/api/courses/{courseId}/modules` → List modules in course  
- **GET** `/api/courses/{courseId}/modules/{moduleId}` → Get module details  
- **POST** `/api/courses/{courseId}/modules` → Create module  
- **PUT** `/api/courses/{courseId}/modules/{moduleId}` → Update module  

### Lessons

- **GET** `/api/courses/{courseId}/modules/{moduleId}/lessons` → List lessons  
- **GET** `/api/courses/{courseId}/modules/{moduleId}/lessons/{lessonId}` → Get lesson  
- **POST** `/api/courses/{courseId}/modules/{moduleId}/lessons` → Create lesson  
- **PUT** `/api/courses/{courseId}/modules/{moduleId}/lessons/{lessonId}` → Update lesson  

### Quizzes

- **GET** `/api/lessons/{lessonId}/quizzes` → List quizzes in lesson  
- **POST** `/api/lessons/{lessonId}/quizzes` → Create quiz  
- **PUT** `/api/lessons/{lessonId}/quizzes/{quizId}` → Update quiz  

### Questions & Options

- **GET** `/api/quizzes/{quizId}/questions` → List questions  
- **POST** `/api/quizzes/{quizId}/questions` → Create question  
- **PUT** `/api/quizzes/{quizId}/questions/{questionId}` → Update question  
- **POST** `/api/quizzes/{quizId}/questions/{questionId}/options` → Add option  
- **PUT** `/api/quizzes/{quizId}/questions/{questionId}/options/{optionId}` → Update option  

### Assignments

- **GET** `/api/lessons/{lessonId}/assignments` → List assignments  
- **GET** `/api/lessons/{lessonId}/assignments/{assignmentId}` → Get assignment  
- **POST** `/api/lessons/{lessonId}/assignments` → Create assignment  
- **PUT** `/api/lessons/{lessonId}/assignments/{assignmentId}` → Update assignment  
- **POST** `/api/lessons/{lessonId}/assignments/{assignmentId}/submit` → Submit assignment  
- **POST** `/api/lessons/{lessonId}/assignments/{assignmentId}/submissions/{submissionId}/grade` → Grade submission  

### File Uploads

- **POST** `/api/files/upload/single` → Upload single file (multipart/form-data)  
- **POST** `/api/files/upload/batch` → Upload multiple files  
- **GET** `/api/files/hehe` → Test file service endpoint (dev only)  

---
