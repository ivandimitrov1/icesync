FROM node:16-alpine as build

ARG REACT_APP_SYNC_APP_BASE_API_URL

# Set them as environment variables for React
ENV REACT_APP_SYNC_APP_BASE_API_URL=${REACT_APP_SYNC_APP_BASE_API_URL}

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm install

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/build /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 3000

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]