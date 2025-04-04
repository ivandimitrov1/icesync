export const fetchWorkflows = async () => {
  try {
    const response = await fetch(`${process.env.REACT_APP_SYNC_APP_BASE_API_URL}/api/workflows`);
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return await response.json();
  } catch (error) {
    throw error;
  }
};

export const runWorkflow = async (workflowId) => {
  try {
    const response = await fetch(`${process.env.REACT_APP_SYNC_APP_BASE_API_URL}/api/workflows/${workflowId}/run`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response.ok;
  } catch (error) {
    throw error;
  }
};