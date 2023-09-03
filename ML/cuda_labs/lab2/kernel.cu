#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>


static void CatchError(cudaError_t err, const char* file, int line) {
    if (err != cudaSuccess) {
        printf("%s in %s at line %d\n", cudaGetErrorString(err),
            file, line);
        exit(EXIT_FAILURE);
    }
}
#define CATCH_ERROR( err ) (CatchError( err, __FILE__, __LINE__ ))


int* find_multipliers(long long int N)
{
    int k;
    int* temp = (int*)malloc(100);
    for (int i = 2; i * i <= N; i++)
    {
        // if composite number:
        if (N % i == 0)
        {
            temp[0] = i;
            printf("%d^", i);
            // calc how many same numbers in composite num
            for (k = 0; N % i == 0; k++)
            {
                temp[k + 1] = k;
                N /= i;
            }
            printf("%d ", k);
        }
    }
    if (N > 1) {
        temp[0] = N;
        printf("%d ", N);
    };
    printf("\n");
    return temp;
}


// func for CPU calculations
void my_find(int** res, long long int n) {
    long long int tid = 0;
    
    while (tid<n)
    {
        res[tid] = find_multipliers(tid);
        tid++;    }
}


__device__ int* find_multipliers_gpu(long long int N)
{
    int k;
    int* temp = (int*)malloc(100);
    for (int i = 2; i * i <= N; i++)
    {
        // if composite number:
        if (N % i == 0)
        {
            printf("%d^", i);
            temp[0] = i;
            // calc how many same numbers in composite num
            for (k = 0; N % i == 0; k++)
            {
                N /= i;
            }
            temp[k + 1] = k;
            printf("%d ", k);
        }
    }
    if (N > 1) 
    {
        temp[0] = N;
        printf("%d ", N);
    }
    printf("\n");
    return temp;
}


__global__ void kernel(int **res, long long int n)
{
    long long int tid = blockIdx.x * blockDim.x + threadIdx.x;
    long long int offset = blockDim.x * gridDim.x;

    
    while (tid < n) {
        res[tid] = find_multipliers_gpu(tid);
        tid += offset;
    }
}



int main()
{
    long long int n = 1024;


    int count;
    CATCH_ERROR(cudaGetDeviceCount(&count));
    if (count == 0) {
        printf("there is no cuda device");
        return -1;
    }


    // for timer
    cudaEvent_t time_of_start, time_of_end;
    float res_timer_gpu;
    CATCH_ERROR(cudaEventCreate(&time_of_start));
    CATCH_ERROR(cudaEventCreate(&time_of_end));


    int** res = (int**)malloc(n * 1000 * sizeof(int*));

    int** res_dev;
    CATCH_ERROR(cudaMalloc(&res_dev, n * 1000 * sizeof(int*)));
    CATCH_ERROR(cudaEventRecord(time_of_start)); // start of timer (GPU)

    kernel << <256, 256 >> > (res_dev, n);

    CATCH_ERROR(cudaEventRecord(time_of_end)); // end of timer (GPU)
    CATCH_ERROR(cudaEventSynchronize(time_of_end));
    
    CATCH_ERROR(cudaEventElapsedTime(&res_timer_gpu, time_of_start, time_of_end));

    CATCH_ERROR(cudaEventDestroy(time_of_start));
    CATCH_ERROR(cudaEventDestroy(time_of_end));

    CATCH_ERROR(cudaMemcpy(res, res_dev, n * sizeof(int), cudaMemcpyDeviceToHost));

    CATCH_ERROR(cudaFree(res_dev));


    free(res);

    res = (int**)malloc(n * n * sizeof(int));
    double res_timer_cpu = 0.0;

    clock_t begin = clock();

    my_find(res, n);

    clock_t end = clock();
        
    res_timer_cpu += (double)(end - begin) / CLOCKS_PER_SEC;

    printf("Results by CPU: %f\n", res_timer_cpu * 1000);

    printf("Results by GPU: %f\n", res_timer_gpu);

    free(res);
    return 0;
}
