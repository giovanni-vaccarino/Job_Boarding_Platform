import { useNavigate } from 'react-router-dom';

export const addParamsToPath = (
  path: string,
  params: Record<string, string>
) => {
  return Object.keys(params).reduce((acc, key) => {
    const regex = new RegExp(`(:${key})[?]?`);
    return acc.replace(regex, params[key]);
  }, path);
};

/**
 * Wrapper for react-router-dom's useNavigate hook that adds the current language to the path,  also add params to the path
 * @returns navigate function
 * @example
 * const navigate = useNavigateWrapper();
 * navigate('/path/:id', { id: '1' });
 *
 */
export const useNavigateWrapper = () => {
  const navigate = useNavigate();

  return (path: string, params?: Record<string, string>) => {
    const updatedParams = params || {};

    const newPath = addParamsToPath(path, updatedParams);

    navigate(newPath);
  };
};
