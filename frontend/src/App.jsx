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
const CustomerListView = lazy(() => import("features/Customer/CustomerListView"));
const CustomerAddEditView = lazy(() => import("features/Customer/CustomerAddEditView"));
const OrganizationListView = lazy(() => import("features/Organization/OrganizationListView"));
const OrganizationAddEditView = lazy(() => import("features/Organization/OrganizationAddEditView"));
const OrganizationContactListView = lazy(() => import("features/OrganizationContact/OrganizationContactListView"));
const OrganizationContactAddEditView = lazy(() => import("features/OrganizationContact/OrganizationContactAddEditView"));
const OrganizationRequisiteListView = lazy(() => import("features/OrganizationRequisite/OrganizationRequisiteListView"));
const OrganizationRequisiteAddEditView = lazy(() => import("features/OrganizationRequisite/OrganizationRequisiteAddEditView"));
const OrganizationTypeListView = lazy(() => import("features/OrganizationType/OrganizationTypeListView"));
const OrganizationTypeAddEditView = lazy(() => import("features/OrganizationType/OrganizationTypeAddEditView"));
// const RepresentativeListView = lazy(() => import("features/Representative/RepresentativeListView"));
// const RepresentativeAddEditView = lazy(() => import("features/Representative/RepresentativeAddEditView"));
// const RepresentativeContactListView = lazy(() => import("features/RepresentativeContact/RepresentativeContactListView"));
// const RepresentativeContactAddEditView = lazy(() => import("features/RepresentativeContact/RepresentativeContactAddEditView"));
// const RepresentativeTypeListView = lazy(() => import("features/RepresentativeType/RepresentativeTypeListView"));
// const RepresentativeTypeAddEditView = lazy(() => import("features/RepresentativeType/RepresentativeTypeAddEditView"));

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
              { 
                path: "Customer", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <CustomerListView />
                  </Suspense>
                ) 
              },
              { 
                path: "Customer/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <CustomerAddEditView />
                  </Suspense>
                ) 
              },
              {path: "Organization", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationListView />
                  </Suspense>
              )},
              {path: "Organization/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationAddEditView />
                  </Suspense>
              )},
              {path: "OrganizationContact", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationContactListView />
                  </Suspense>
              )},
              {path: "OrganizationContact/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationContactAddEditView />
                  </Suspense>
              )},
              {path: "OrganizationRequisite", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationRequisiteListView />
                  </Suspense>
              )},
              {path: "OrganizationRequisite/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationRequisiteAddEditView />
                  </Suspense>
              )},
              {path: "OrganizationType", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationTypeListView />
                  </Suspense>
              )},
              {path: "OrganizationType/addedit", 
                element: (
                  <Suspense fallback={<LoadingFallback />}>
                    <OrganizationTypeAddEditView />
                  </Suspense>
              )},
              // {path: "Representative", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeListView />
              //     </Suspense>
              // )},
              // {path: "Representative/addedit", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeAddEditView />
              //     </Suspense>
              // )},
              // {path: "RepresentativeContact", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeContactListView />
              //     </Suspense>
              // )},
              // {path: "RepresentativeContact/addedit", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeContactAddEditView />
              //     </Suspense>
              // )},
              // {path: "RepresentativeType", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeTypeListView />
              //     </Suspense>
              // )},
              // {path: "RepresentativeType/addedit", 
              //   element: (
              //     <Suspense fallback={<LoadingFallback />}>
              //       <RepresentativeTypeAddEditView />
              //     </Suspense>
              // )},
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