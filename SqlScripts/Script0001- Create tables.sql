CREATE TABLE public.test (
        id varchar NOT NULL,
        email varchar NOT NULL,
        username varchar NOT NULL,
        firstname varchar NULL,
        lastname varchar NULL,
        passwordhash varchar NOT NULL,
        isactivated bool NULL DEFAULT false,
        createdat date NULL,
        updatedat date NULL
);
