# Deployment Guide - Products Microservice

This guide provides step-by-step instructions for deploying the Products microservice to Kubernetes using Docker and Helm.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Quick Start](#quick-start)
3. [Local Development (Minikube)](#local-development-minikube)
4. [Production Deployment](#production-deployment)
5. [Verification](#verification)
6. [Troubleshooting](#troubleshooting)
7. [Updating the Deployment](#updating-the-deployment)

## Prerequisites

- Docker (20.10+)
- Kubernetes cluster (1.19+)
- kubectl configured to access your cluster
- Helm 3.0+
- (Optional) Minikube for local testing

## Quick Start

For a quick deployment to an existing Kubernetes cluster:

```bash
# 1. Build the Docker image
cd /path/to/HomeOrganizer
docker build -f Products.Api/Dockerfile -t products-api:1.0.0 .

# 2. Tag and push to your registry
docker tag products-api:1.0.0 your-registry.io/products-api:1.0.0
docker push your-registry.io/products-api:1.0.0

# 3. Deploy with Helm
helm install products k8s/products-service/ \
  --set image.repository=your-registry.io/products-api \
  --set image.tag=1.0.0

# 4. Verify deployment
kubectl get pods -l app.kubernetes.io/name=products-service
kubectl get svc -l app.kubernetes.io/name=products-service
```

## Local Development (Minikube)

### 1. Start Minikube

```bash
minikube start --cpus=4 --memory=8192
```

### 2. Build Image in Minikube

```bash
# Point Docker to Minikube's Docker daemon
eval $(minikube docker-env)

# Build the image
cd /path/to/HomeOrganizer
docker build -f Products.Api/Dockerfile -t products-api:latest .
```

### 3. Deploy with Development Values

```bash
helm install products k8s/products-service/ \
  -f k8s/products-service/values-dev.yaml
```

### 4. Access the Service

```bash
# Port forward to access locally
kubectl port-forward svc/products-products-service 8080:80

# Or use minikube service
minikube service products-products-service
```

### 5. Test the Health Endpoint

```bash
curl http://localhost:8080/health
```

## Production Deployment

### 1. Prepare Your Environment

```bash
# Set your registry
REGISTRY="your-registry.azurecr.io"
IMAGE_NAME="products-api"
VERSION="1.0.0"
```

### 2. Build and Push Image

```bash
# Build
docker build -f Products.Api/Dockerfile -t ${REGISTRY}/${IMAGE_NAME}:${VERSION} .

# Login to registry
docker login ${REGISTRY}

# Push
docker push ${REGISTRY}/${IMAGE_NAME}:${VERSION}
```

### 3. Create Secrets (if needed)

```bash
# Create image pull secret (for private registries)
kubectl create secret docker-registry registry-secret \
  --docker-server=${REGISTRY} \
  --docker-username=your-username \
  --docker-password=your-password \
  --docker-email=your-email@example.com

# Create application secrets (example for database connection)
kubectl create secret generic products-secrets \
  --from-literal=database-connection-string="your-connection-string"
```

### 4. Customize Values

Create a custom values file `values-production.yaml`:

```yaml
replicaCount: 3

image:
  repository: your-registry.azurecr.io/products-api
  pullPolicy: IfNotPresent
  tag: "1.0.0"

imagePullSecrets:
  - name: registry-secret

resources:
  limits:
    cpu: 1000m
    memory: 1Gi
  requests:
    cpu: 500m
    memory: 512Mi

autoscaling:
  enabled: true
  minReplicas: 3
  maxReplicas: 20
  targetCPUUtilizationPercentage: 70

ingress:
  enabled: true
  className: "nginx"
  annotations:
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
  hosts:
    - host: products.yourdomain.com
      paths:
        - path: /
          pathType: Prefix
  tls:
    - secretName: products-tls
      hosts:
        - products.yourdomain.com

env:
  - name: ASPNETCORE_ENVIRONMENT
    value: "Production"
  - name: ConnectionStrings__DefaultConnection
    valueFrom:
      secretKeyRef:
        name: products-secrets
        key: database-connection-string
```

### 5. Deploy

```bash
helm install products k8s/products-service/ \
  -f values-production.yaml \
  --namespace production \
  --create-namespace
```

## Verification

### Check Deployment Status

```bash
# Check pods
kubectl get pods -l app.kubernetes.io/name=products-service

# Check service
kubectl get svc -l app.kubernetes.io/name=products-service

# Check ingress (if enabled)
kubectl get ingress

# View logs
kubectl logs -l app.kubernetes.io/name=products-service --tail=100
```

### Health Check

```bash
# Port forward
kubectl port-forward svc/products-products-service 8080:80

# Test health endpoint
curl http://localhost:8080/health

# Expected response: Healthy
```

### Check Resource Usage

```bash
# Check pod resource usage
kubectl top pods -l app.kubernetes.io/name=products-service

# Check HPA status (if enabled)
kubectl get hpa
```

## Troubleshooting

### Pod Not Starting

```bash
# Describe pod to see events
kubectl describe pod <pod-name>

# Check logs
kubectl logs <pod-name>

# Check previous container logs (if crashed)
kubectl logs <pod-name> --previous
```

### Image Pull Errors

```bash
# Verify image exists
docker pull your-registry.io/products-api:1.0.0

# Check image pull secret
kubectl get secret registry-secret -o yaml

# Verify secret is attached to service account
kubectl describe serviceaccount products-products-service
```

### Health Check Failing

```bash
# Port forward to test manually
kubectl port-forward <pod-name> 8080:8080

# Test health endpoint
curl http://localhost:8080/health

# Check logs for startup issues
kubectl logs <pod-name>
```

### Service Not Accessible

```bash
# Check endpoints
kubectl get endpoints

# Check service
kubectl describe svc products-products-service

# Verify pod labels match service selector
kubectl get pods --show-labels
```

## Updating the Deployment

### Update Image Version

```bash
# Build and push new version
docker build -f Products.Api/Dockerfile -t ${REGISTRY}/${IMAGE_NAME}:1.1.0 .
docker push ${REGISTRY}/${IMAGE_NAME}:1.1.0

# Upgrade deployment
helm upgrade products k8s/products-service/ \
  --set image.tag=1.1.0 \
  --reuse-values
```

### Update Configuration

```bash
# Edit your values file, then upgrade
helm upgrade products k8s/products-service/ \
  -f values-production.yaml
```

### Rollback Deployment

```bash
# View release history
helm history products

# Rollback to previous version
helm rollback products

# Or rollback to specific revision
helm rollback products 2
```

### Uninstall

```bash
# Uninstall release
helm uninstall products

# Verify cleanup
kubectl get all -l app.kubernetes.io/name=products-service
```

## Monitoring and Logging

### View Real-time Logs

```bash
# Follow logs from all pods
kubectl logs -f -l app.kubernetes.io/name=products-service

# Stern (if installed) - better log aggregation
stern products-service
```

### Export Logs

```bash
# Export logs to file
kubectl logs -l app.kubernetes.io/name=products-service > products-logs.txt
```

### Metrics

```bash
# Install metrics server (if not already installed)
kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml

# View resource usage
kubectl top pods -l app.kubernetes.io/name=products-service
kubectl top nodes
```

## Best Practices

1. **Use Specific Image Tags**: Never use `latest` in production
2. **Set Resource Limits**: Always define CPU and memory limits
3. **Enable Autoscaling**: Use HPA for production workloads
4. **Use Secrets**: Never hardcode sensitive data
5. **Health Checks**: Always implement proper health and readiness probes
6. **Monitoring**: Set up logging and monitoring solutions
7. **Backup**: Regularly backup your Helm values files
8. **Testing**: Test in staging environment before production
9. **Version Control**: Keep Helm values in version control
10. **Documentation**: Document any custom configurations

## Next Steps

- Set up CI/CD pipeline for automated deployments
- Configure monitoring with Prometheus and Grafana
- Set up centralized logging with ELK or similar
- Implement distributed tracing
- Configure backup and disaster recovery
- Set up alerts for critical metrics

## Additional Resources

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [Helm Documentation](https://helm.sh/docs/)
- [ASP.NET Core on Kubernetes](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/kubernetes)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
