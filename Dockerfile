# build stage
FROM node:22-alpine as build-stage
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build
## các bạn có thể dùng yarn install .... tuỳ nhu cầu nhé

# production stage
FROM nginx:1.27-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]