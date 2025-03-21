FROM nginx:latest

# This is the media for NGINX to serve static and uploaded files
VOLUME /etc/nginx/multimedia

# Copy the main configuration
COPY prod.conf /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/nginx.conf
