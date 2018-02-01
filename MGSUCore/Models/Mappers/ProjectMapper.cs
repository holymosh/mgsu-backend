using Common.Entities;

namespace MGSUCore.Models.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectModel ProjectToProjectModel(Project project)
        {
            if (project == null) return null;

            return new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Direction = project.Direction,
                ShortDescription = project.ShortDescription,
                Content = project.Content,
				Img = project.Img == null
					? null
					: new ImageModel
                {
                    Original = project.Img.Original,
                    Small = project.Img.Small,
                    Role = project.Img.Role
                },
                Need = project.Need,
                Given = project.Given,
                Public = project.Public,
                CreatingDate = project.CreatingDate
            };
        }

        public static Project ProjectModelToProject(ProjectModel projectModel)
        {
            if (projectModel == null) return null;

            return new Project
            {
				Name = projectModel.Name,
				Direction = projectModel.Direction,
				ShortDescription = projectModel.ShortDescription,
				Content = projectModel.Content,
				Img = projectModel.Img == null
					? null
					: new Image
					{
						Original = projectModel.Img.Original,
						Small = projectModel.Img.Small,
						Role = projectModel.Img.Role
					},
				Need = projectModel.Need,
				Given = projectModel.Given,
				Public = true
            };
        }

    }
}