FROM node:24-alpine

WORKDIR /app

# Enable pnpm and install dependencies
RUN corepack enable && corepack prepare pnpm@latest --activate
COPY pnpm-lock.yaml package.json ./
RUN pnpm install --frozen-lockfile

# Copy source and build
COPY . .
RUN pnpm build && pnpm prune --prod

ENV PORT=3000
EXPOSE 3000

CMD ["sh", "-c", "pnpm preview --host --port $PORT"]
