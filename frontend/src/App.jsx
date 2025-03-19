import { Suspense, lazy } from "react";
import { Outlet, RouterProvider, createBrowserRouter } from "react-router-dom";

import { ThemeProvider } from "@mui/material/styles";
import { StyledEngineProvider } from "@mui/material";
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';

import NavigationScroll from "layouts/NavigationScroll";
import MainWrapper from "layouts/MainWrapper";
import PrivateRoute from "./PrivateRoute";
import PublicRoute from "./PublicRoute";
import themes from "themes";
import MainLayout from "layouts/MainLayout";

// Lazy-loaded components
const AuthForm = lazy(() => import("features/Auth/AuthForm"));
const ServiceListView = lazy(() => import("features/Service/ServiceListView"));
const ServiceAddEditView = lazy(() => import("features/Service/ServiceAddEditView"));

// Loading fallback component
const LoadingFallback = () => (
  <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
    <CircularProgress />
  </Box>
);

const router = createBrowserRouter([
  {
    element: (
      <MainWrapper>
        <Outlet />
      </MainWrapper>
    ),
    children: [
      {
        element: <PrivateRoute />,
        children: [
          {
            element: <MainLayout />,
            path: "/user",
            children: [
              { 
                path: "Service", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <ServiceListView />
                  </Suspense>
                ) 
              },
              { 
                path: "Service/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <ServiceAddEditView />
                  </Suspense>
                ) 
              },
            ]
          }]
      },
      {
        element: <PublicRoute />,
        children: [
          { 
            path: "/login", 
            element: (
              <Suspense fallback={<LoadingFallback />}>
                <AuthForm />
              </Suspense>
            ) 
          },
          { 
            path: "/", 
            element: (
              <Suspense fallback={<LoadingFallback />}>
                <AuthForm />
              </Suspense>
            ) 
          },
        ]
      },
      { path: "error-404", element: <div></div> },
      { path: "access-denied", element: <div></div> },
      { path: "*", element: <div>not founded</div> }
    ]
  }
]);

const App = () => {
  return <StyledEngineProvider injectFirst>
    <ThemeProvider theme={themes(null)}>
      <NavigationScroll>
        <RouterProvider router={router} />
      </NavigationScroll>
    </ThemeProvider>
  </StyledEngineProvider>;
};

export default App;