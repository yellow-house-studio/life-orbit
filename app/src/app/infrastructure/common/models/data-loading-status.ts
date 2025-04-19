export interface DataLoadingStatus {
  isLoading: boolean;
  haveLoaded: boolean;
  haveError: boolean;
  errorMessage: string;
  statusMessage?: string;
}

export const initialDataLoadingStatus: DataLoadingStatus = {
  isLoading: false,
  haveLoaded: false,
  haveError: false,
  errorMessage: '',
};

const createDataLoadingStatus = (
  status: Partial<DataLoadingStatus>
): DataLoadingStatus => {
  return {
    ...initialDataLoadingStatus,
    ...status,
  };
};

export const dataLoadingStatus = createDataLoadingStatus({ isLoading: true });

export const dataLoadingStatusWithMessage = (statusMessage: string) =>
  createDataLoadingStatus({ isLoading: true, statusMessage });

export const dataLoadedStatus = createDataLoadingStatus({ haveLoaded: true });

export const dataErrorStatus = (messageOrError?: string | Error) =>
  createDataLoadingStatus({
    haveError: true,
    errorMessage: messageOrError instanceof Error ? messageOrError.message : messageOrError ?? ''
  });
