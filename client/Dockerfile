FROM node:24-alpine

WORKDIR /app

# Enable pnpm and install dependencies
RUN corepack enable && corepack prepare pnpm@latest --activate
COPY pnpm-lock.yaml package.json ./
RUN pnpm install --frozen-lockfile

ARG VITE_BACKEND_URL
ARG VITE_CLIENT_URL
ARG VITE_MANAGER_URL

ENV VITE_BACKEND_URL=$VITE_BACKEND_URL
ENV VITE_CLIENT_URL=$VITE_CLIENT_URL
ENV VITE_MANAGER_URL=$VITE_MANAGER_URL

# Copy source and build
COPY . .
RUN pnpm build

ENV PORT=3000
EXPOSE 3000

CMD ["sh", "-c", "pnpm preview --host --port $PORT"]
